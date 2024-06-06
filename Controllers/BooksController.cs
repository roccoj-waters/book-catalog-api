using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using BookCatalogApi.Infrastructure;
using BookCatalogApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookCatalogApi.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/books")]
    public class BooksController(BookRepository languageRepository) : Controller
    {
        private readonly BookRepository _bookRepository = languageRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var results = await _bookRepository.GetAllAsync(cancellationToken);

            return Ok(results);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook, CancellationToken cancellationToken)
        {
            await _bookRepository.CreateAsync(newBook, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody]Book updatedBook, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _bookRepository.UpdateAsync(id, updatedBook, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public async Task<IActionResult> Patch(string id, [FromBody]JsonPatchDocument<Book> patchDocument, CancellationToken cancellationToken)
        {
            if (patchDocument is null)
                return BadRequest();

            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(book, ModelState);

            if (!TryValidateModel(book))
            {
                return BadRequest(ModelState);
            }

            await _bookRepository.UpdateAsync(id, book, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return NotFound();
            }

            await _bookRepository.RemoveAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
