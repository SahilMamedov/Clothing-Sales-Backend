using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.CommentDtos;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;


        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(CommentCreateDto commentCreateDto)
        {

            Comment comment = new Comment()
            {
                CreateTime =DateTime.Now,
                AppUserId = commentCreateDto.AppUserId,
                Content = commentCreateDto.Content,
                ProductId = commentCreateDto.ProductId

            };


            _context.Add(comment);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return StatusCode(200);
        }
        [HttpGet("{id}")]
        public IActionResult GetAll(int id)
        {
            List<Comment> comments = _context.Comments.Include(c => c.AppUser).Where(c => c.ProductId == id).ToList();

            return Ok(comments);
        }
    }
}
