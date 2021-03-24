using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipDbListApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ShipDbListApplication.Controllers {
    public class ShipListController : Controller {

        private readonly ShipDbContext db;
        [BindProperty]
        public Ships row { get; set; }

        public ShipListController(ShipDbContext db) {
            this.db = db;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Updater(int? id) {
            row = new Ships();
            if (id == null) {
                return View(row);
            }
            row = db.ShipDbs.FirstOrDefault(u => u.Id == id);
            if (row == null) {
                return NotFound();
            }
            return View(row);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Updater() {
            if (ModelState.IsValid) {
                if (row.Id == 0) {
                    db.ShipDbs.Add(shipRow(row));
                } else {
                    db.ShipDbs.Update(shipRow(row));
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(row);


        }

        private List<Ships> formalizeShipTable(List<Ships> data) {
            for (int i = 0; i < data.Count; i++) {
                Ships row = shipRow(data[i]);
                db.ShipDbs.Update(row);
                data[i] = row;
            }
            db.SaveChangesAsync();
            return data;
        }

        private Ships shipRow(Ships row) {
            row.Type = char.ToUpper(row.Type);
            row.Type = row.Type > 'D' || row.Type < 'A' ? 'D' : row.Type;
            row.Class = char.ToUpper(row.Class);
            row.Class = (char.IsDigit(row.Class) || char.IsLetter(row.Class)) ? row.Class : ' ';
            row.HardPoints = Math.Max(row.HardPoints, 0);
            row.TypeString = row.Type == 'D' ? "Delta" : row.Type == 'C' ? "Charlie" : row.Type == 'A' ? "Alpha" : row.Type == 'B' ? "Bravo" : "None";
            row.ClassString = row.Class == 'F' ? "Fighter" : row.Class == 'B' ? "Bomber" : row.Class == 'A' ? "Attack" : row.Class == 'X' ? "Expiramental" : "Unknown";
            return row;
        }


        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetShipsDb() {
            List<Ships> data = await db.ShipDbs.ToListAsync();
            data = formalizeShipTable(data);
            return Json(new { data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteShip(int id) {
            var ro = await db.ShipDbs.FirstOrDefaultAsync(u => u.Id == id);
            if (ro == null) {
                return Json(new { success = false, message = "Failure" });
            }
            db.ShipDbs.Remove(ro);
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Success!" });
        }

        [HttpGet]
        public async Task<IActionResult> CloneShip(int id) {
            row = await db.ShipDbs.FirstOrDefaultAsync(u => u.Id == id);
            if (row == null) {
                return Json(new { success = false, message = "Failure" });
            }
            db.ShipDbs.Add(new Ships { Type = row.Type, Class = row.Class, HardPoints = row.HardPoints, Weight = row.Weight, ShipName = row.ShipName });
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Success!" });
        }
        #endregion


        private Ships newRow(Ships row) {
            Ships ro = new Ships();
            return ro;
        }
    }
}
