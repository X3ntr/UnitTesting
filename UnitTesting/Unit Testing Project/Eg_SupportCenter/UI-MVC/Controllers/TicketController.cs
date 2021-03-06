﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Models;
namespace SC.UI.Web.MVC.Controllers
{
    public class TicketController : Controller
    {
        private ITicketManager mgr;

        //implemented contructor to allow overloading to testing
        public TicketController()
        {
            mgr = new TicketManager();
        }

        //overloaded constructor to inject TicketManager
        public TicketController(ITicketManager mgr)
        {
            this.mgr = mgr;
        }

        // GET: Ticket
        public ActionResult Index()
        {
            IEnumerable<Ticket> tickets = mgr.GetTickets();
            return View("Index", tickets);
        }

        // GET: Ticket/Details/5
        public ActionResult Details(int id)
        {
            Ticket ticket = mgr.GetTicket(id);

            var responses = mgr.GetTicketResponses(id).ToList();

            ticket.Responses = responses;
            // OF: via ViewBag
            //ViewBag.Responses = responses;

            return View("Details", ticket); //explicitely specify view name to use in Unit Test
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Ticket/Create
        [HttpPost]
        /* public ActionResult Create(Ticket ticket)
        {
          if (ModelState.IsValid)
          {
            ticket = mgr.AddTicket(ticket.AccountId, ticket.Text);

            return RedirectToAction("Details", new { id = ticket.TicketNumber });
          }

          return View();
        } */
        // ADHV view-model 'CreateTicketVM'
        public ActionResult Create(CreateTicketVM newTicket)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = mgr.AddTicket(newTicket.AccId, newTicket.Problem);

                return RedirectToAction("Details", new { id = ticket.TicketNumber });
            }

            return View("Create");
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            Ticket ticket = mgr.GetTicket(id);
            return View("Edit", ticket);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                mgr.ChangeTicket(ticket);

                return RedirectToAction("Index");
            }

            return View("Edit");
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(int id)
        {
            Ticket ticket = mgr.GetTicket(id);
            return View("Delete", ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                mgr.RemoveTicket(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete");
            }
        }
    }
}
