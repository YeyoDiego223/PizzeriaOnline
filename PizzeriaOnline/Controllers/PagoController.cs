using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzeriaOnline.Data;
using Stripe.Checkout;

namespace PizzeriaOnline.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PagoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult CrearCheckoutSession(int pedidoId)
        {
            var pedido = _context.Pedidos.Include(p => p.Detalles).FirstOrDefault(p => p.Id == pedidoId);
            if (pedido == null)
            {
                return NotFound();
            }

            // Configura tu clave secreta de Stripe
            Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var lineItems = new List<SessionLineItemOptions>();

            // Añade cada detalle del pedido como una línea de producto para Stripe
            foreach(var detalle in pedido.Detalles)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(detalle.PrecioUnitario * 100), // Precio en centavos
                        Currency = "mxn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = detalle.NombreTamaño ?? "Producto Extra"
                        },
                    },
                    Quantity = detalle.Cantidad,
                });
            }

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = Url.Action("PagoExitoso", "Pago", new { id = pedido.Id }, Request.Scheme),
                CancelUrl = Url.Action("PagoCancelado", "Pago", new { id = pedido.Id }, Request.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }
       
        // Esta acción se ejecuta ccuando Stripe nos confirma un pago exitoso
        public IActionResult PagoExitoso(int id)
        {
            // Buscamos el pedido en nuestra base de datos
            var pedido = _context.Pedidos.Find(id);
            if (pedido != null)
            {
                // Actualizamo su estado a "Recibido"
                pedido.Estado = "Recibido";
                _context.SaveChanges();
            }

            // Pasamos el ID del pedido a la vista para mostrar un mensaje de confirmación
            ViewBag.PedidoId = id;
            return View();
        }

        // Esta acción se ejecuta si el cliente cancela el pago en la página de Stripe
        public IActionResult PagoCancelado(int id)
        {
            // Simplemente mostramos una página informando que el pago fue cancelado.
            // El pedido permanecera en estado "Pendiente de Pago".
            ViewBag.PedidoId = id;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
