﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ServiceInterface;
using AutoMapper;
using Autofac;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Hubs
{
    
    public class DashBoardHub : Hub
    {
        private readonly ILifetimeScope _hubLifetimeScope;
        IStoreServerService _IStoreServerService;
        private readonly IMapper _mapper;

        public DashBoardHub(IStoreServerService iStoreServerService, IMapper mapper, ILifetimeScope lifetimeScope)
        {
            this._IStoreServerService = iStoreServerService;
            _mapper = mapper;
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

        }

        public override Task OnConnected()
        {

            return base.OnConnected();


        }

        public void testt()
        {

            Clients.All.Test("3000", "2000");

        }

        public void UpdateDashBoardHub()
        {

        }

        protected override void Dispose(bool disposing)
        {
            // Dipose the hub lifetime scope when the hub is disposed.
            if (disposing && _hubLifetimeScope != null)
            {
                _hubLifetimeScope.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
