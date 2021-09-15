using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarApplication.Data;
using CarApplication.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Web.Helpers;
using System.Net;
using Nancy.Json;

namespace ratiap.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment )
        {
            _context = context;

            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }
        

        public decimal GetCurency(String type)
        {
            var url ="https://free.currconv.com/api/v7/convert?q="+ type+"&compact=ultra&apiKey=e4a69cf67f3686abac42";

            var webClient = new WebClient();

            var jsonData = String.Empty;

            var conversionRate = 1.0m;

            try
            {
                jsonData = webClient.DownloadString(url);

                var jsonObject = new JavaScriptSerializer().Deserialize<Dictionary<string, Dictionary<string, decimal>>>(jsonData);

                var result = jsonObject[type];

                conversionRate = result["val"];


            }
            catch (Exception)
            {

                return conversionRate;
            }

            return conversionRate;
        }



        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarBrand,Description,Price,RealeaseYear,Abs,Elshushebi,Luqi,Bluetooth,Signal,ParkingControl,Navigation,BortComputer,MultiWheel,CarPhoto,priceInGel")] Car car)
        {
           decimal factor = GetCurency("GEL_USD");

            car.priceInGel = (int)(car.Price * factor);
           
             string AddPhoto([FromForm] Car Image)
            {
                try
                {
                    if(Image.CarPhoto.Length>0)
                    {
                        string path = _webHostEnvironment.WebRootPath + "\\Images\\Photos\\";

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (FileStream fileStream = System.IO.File.Create(path + Image.CarPhoto.FileName))
                        {
                            Image.CarPhoto.CopyTo(fileStream);
                            fileStream.Flush();

                            car.imgurl = Image.CarPhoto.FileName;

                            return "Uploaded";
                        }
                    }
                    else
                    {
                        return "Not uploaded";
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            AddPhoto(car);

            if (ModelState.IsValid)
            {
               
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);

        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarBrand,Description,Price,RealeaseYear,Abs,Elshushebi,Luqi,Bluetooth,Signal,ParkingControl,Navigation,BortComputer,MultiWheel,imgurl")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
