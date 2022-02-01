using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Entities;
using CalculationsLibrary;

namespace API.Controllers
{
    public class CalculationsController : ApiController
    {
        private Furniture_storeEntities db = new Furniture_storeEntities();

        // GET: api/Calculations
        public IQueryable<Calculation> GetCalculation()
        {
            return db.Calculation;
        }

        // GET: api/Calculations/5
        [ResponseType(typeof(Calculation))]
        public IHttpActionResult GetCalculation(int id)
        {
            Calculation calculation = db.Calculation.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            return Ok(calculation);
        }

        // PUT: api/Calculations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalculation(int id, Calculation calculation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calculation.Id)
            {
                return BadRequest();
            }

            db.Entry(calculation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalculationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Calculations
        [ResponseType(typeof(Calculation))]
        public IHttpActionResult PostCalculation(Calculation calculation)
        {
            calculation.Result = CalculationsLibrary.Calculations.CalculatingTheCredits(Convert.ToDouble(calculation.Amount), Convert.ToDouble(calculation.NumberMonths));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Calculation.Add(calculation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CalculationExists(calculation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = calculation.Id }, calculation);
        }

        // DELETE: api/Calculations/5
        [ResponseType(typeof(Calculation))]
        public IHttpActionResult DeleteCalculation(int id)
        {
            Calculation calculation = db.Calculation.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            db.Calculation.Remove(calculation);
            db.SaveChanges();

            return Ok(calculation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalculationExists(int id)
        {
            return db.Calculation.Count(e => e.Id == id) > 0;
        }
    }
}