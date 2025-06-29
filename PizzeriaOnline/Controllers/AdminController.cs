﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;



namespace PizzeriaOnline.Controllers
{
    

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int id, string nuevoEstado)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido != null && !string.IsNullOrEmpty(nuevoEstado))
            {
                pedido.Estado = nuevoEstado;
                _context.SaveChanges();
            }

            return RedirectToAction("DetallePedido", new { id = id });
        }

        public IActionResult DetallePedido(int id)
        {
            var pedidoConDetalles = _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefault(p => p.Id == id);

            if (pedidoConDetalles == null)
            {
                return NotFound();
            }

            return View(pedidoConDetalles);
        }
        public IActionResult Index()
        {
            var listaDePedidos = _context.Pedidos.OrderByDescending(p => p.FechaPedido).ToList();
            return View(listaDePedidos);
        }
    }
}
