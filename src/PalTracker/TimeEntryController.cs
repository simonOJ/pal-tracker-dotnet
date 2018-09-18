using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private ITimeEntryRepository  _inMemoryTimeEntryRepository;

        public TimeEntryController(ITimeEntryRepository timeEntry)
        {
             _inMemoryTimeEntryRepository = timeEntry;
        }

       [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(long id)
        {
           return _inMemoryTimeEntryRepository.Contains(id) ? (IActionResult) Ok(_inMemoryTimeEntryRepository.Find(id)) : NotFound();
        }    

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry entry)
        {
            var createdTimeEntry = _inMemoryTimeEntryRepository.Create(entry);
            return CreatedAtRoute("GetTimeEntry", new {id = createdTimeEntry.Id}, createdTimeEntry);
        } 

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_inMemoryTimeEntryRepository.List());
        } 

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry timeEntry)
        {
            return _inMemoryTimeEntryRepository.Contains(id) ? (IActionResult) Ok(_inMemoryTimeEntryRepository.Update(id, timeEntry)) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!_inMemoryTimeEntryRepository.Contains(id))
            {
                return NotFound();
            }

            _inMemoryTimeEntryRepository.Delete(id);

            return NoContent();
        }
    } 
} 
