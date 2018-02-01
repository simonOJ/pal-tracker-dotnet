using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private ITimeEntryRepository  _inMemoryTimeEntryRepository;
        private readonly IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryController(ITimeEntryRepository timeEntry, IOperationCounter<TimeEntry> operationCounter)
        {
             _inMemoryTimeEntryRepository = timeEntry;
             _operationCounter = operationCounter;
        }

       [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(long id)
        {
            _operationCounter.Increment(TrackedOperation.Read);
           return _inMemoryTimeEntryRepository.Contains(id) ? (IActionResult) Ok(_inMemoryTimeEntryRepository.Find(id)) : NotFound();
        }    

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry entry)
        {
            _operationCounter.Increment(TrackedOperation.Create);
            var createdTimeEntry = _inMemoryTimeEntryRepository.Create(entry);
            return CreatedAtRoute("GetTimeEntry", new {id = createdTimeEntry.Id}, createdTimeEntry);
        } 

        [HttpGet]
        public IActionResult List()
        {
            _operationCounter.Increment(TrackedOperation.List);
            return Ok(_inMemoryTimeEntryRepository.List());
        } 

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry timeEntry)
        {
            _operationCounter.Increment(TrackedOperation.Update);
            return _inMemoryTimeEntryRepository.Contains(id) ? (IActionResult) Ok(_inMemoryTimeEntryRepository.Update(id, timeEntry)) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _operationCounter.Increment(TrackedOperation.Delete);
            if (!_inMemoryTimeEntryRepository.Contains(id))
            {
                return NotFound();
            }

            _inMemoryTimeEntryRepository.Delete(id);

            return NoContent();
        }
    } 
} 
