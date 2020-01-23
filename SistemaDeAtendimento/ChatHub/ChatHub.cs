﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.SignalR;
using SistemaDeAtendimento.Entity;
using Microsoft.AspNet.Identity;
using SistemaDeAtendimento.Models;
using SistemaDeAtendimento.Helpers;

namespace SistemaDeAtendimento.ChatHub
{
    public class ChatHub : Hub
    {
        private SistemaAtendimentoEntities db = new SistemaAtendimentoEntities();
        private List<ChatViewModel> modelChat = new List<ChatViewModel>();

        public void ListConsultores()
        {
            var User = db.AspNetRoles.Where(s => s.Name == "Consultor").FirstOrDefault().AspNetUsers.Where(a => a.Status == "Disponível").OrderByDescending(s => s.OrdemRegistros).ToList();
            Clients.All.limpaLista();
            if (User.Count() <= 0)
            {
                Clients.All.listarConsul(false);
            }
            else
            {
                foreach (var item in User)
                {
                    Clients.All.listarConsul(true, item.Foto, item.Nome, item.Descricao, item.Email, item.Id);
                }
            }
        }

        public void Send(string name, string message, string group, bool remetente, int IdConversa)
        {
            try
            {
                var msg = new Mensagens { Mensagem = message, Remetente = remetente, ConversaId = IdConversa, data = DateTime.Now };
                db.Mensagens.Add(msg);
                db.SaveChanges();

                if (remetente)
                {
                    Clients.Group(group).addNewMessageToPageConsultor(name, message);
                } else
                {
                    Clients.Group(group).addNewMessageToPageVisitante(name, message);
                }
            } catch
            {
                Clients.Group("Todos").addNewMessageToPage(name, message);
            }
        }

        public void Timer(string groupName, int tempo)
        {
            TimeSpan result = TimeSpan.FromSeconds(tempo);
            string tempoFinal = result.ToString("mm':'ss");
//<<<<<<< HEAD
//            //Cookies.Set("Tempo", tempo.ToString());
//=======
            //var variavel = modelChat.FirstOrDefault(x => x.consultor == groupName);
            //variavel.Duracao = tempo;

            Clients.Group(groupName).countTimer(tempoFinal, tempo);
        }

        public void Digite(string groupName, string name)
        {
            Clients.Group(groupName).digitandoMessage("O usuário " + name + " está digitando.");
        }

        public void ApagarDigite(string groupName)
        {
            Clients.Group(groupName).apagarDigitandoMessage();
        }
        
        public void ChatLink(string groupName, int? idConversa)
        {
            Clients.Group(groupName).link("https://localhost:44332/Chat/index/" + idConversa);
        }

        public void Upload(string groupId, string name, string filename, int conversa, bool remetente)
        {
            var msg = new Mensagens { Arquivos = "/Chat/Download/?path=" + conversa + "&filename=" + filename, 
                                        Remetente = remetente, 
                                        ConversaId = conversa, 
                                        data = DateTime.Now };
            db.Mensagens.Add(msg);
            db.SaveChanges();

            if (remetente)
            {
                Clients.Group(groupId).addNewLinkFileToPageConsultor(name, filename, conversa);
            } else
            {
                Clients.Group(groupId).addNewLinkFileToPageVisitante(name, filename, conversa);
            }
            
        }

        //public override Task OnDisconnected(bool stopCalled)
        //{


        //    return base.OnDisconnected(stopCalled);
        //}

        public async Task AddToGroup(string groupName, string name)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            
            await Clients.Group(groupName).joinGroup("O usuário " + name + " foi conectado ao chat.");
            
        }
        public async Task RemoveFromGroup(string groupName, string name)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
            await Clients.Group(groupName).leaveGroup("O usuário " + name + " foi desconectado do chat.");
        }
    }
}