using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OpenFaaS
{
    [ApiController]
    [Route("/")]
    public class Controller : ControllerBase
    {
        // not a real thing, naturally, but for education purposes it will do
        private static readonly IDictionary<string, Note> dictionary = new Dictionary<string, Note>();

        /// <summary>
        /// Retrieves a list of notes
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var notes = dictionary.Values.Select( note => new Note
            {
                Id = note.Id
            } )
            .ToArray();

            if ( !notes.Any() )
            {
                // no notes
                return NoContent();
            }

            return Ok( notes );
        }

        /// <summary>
        /// Retrieves a note with the given id
        /// </summary>
        [HttpGet( "{id}" )]
        public IActionResult GetSingle( string id )
        {
            if ( !dictionary.TryGetValue( id, out var note ) )
            {
                // there isn't a note with the given id
                return NotFound();
            }

            return Ok( note );
        }

        /// <summary>
        /// Creates a new note
        /// </summary>
        [HttpPost]
        public IActionResult Post( [FromBody] Note note )
        {
            note.Id = Guid.NewGuid().ToString();

            dictionary.Add( note.Id, note );

            // we should return a 201 Created response here
            // but let's keep it simple as it's not the
            // purpose of the article
            return Ok( note );
        }

        /// <summary>
        /// Replaces the contents of an existing note with the given id
        /// </summary>
        [HttpPut( "{id}" )]
        public IActionResult Put( string id, [FromBody] Note note )
        {
            if ( !dictionary.ContainsKey( id ) )
            {
                // there isn't a note with the given id
                return NotFound();
            }

            note.Id = id;

            dictionary[id] = note;

            return Ok( note );
        }

        [HttpDelete( "{id}" )]
        public IActionResult Delete( string id )
        {
            if ( !dictionary.ContainsKey( id ) )
            {
                // there isn't a note with the given id
                return NotFound();
            }

            dictionary.Remove( id );

            return NoContent();
        }
    }
}
