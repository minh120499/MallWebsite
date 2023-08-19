using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/feedbacks")]
    public class FeedbacksController : ControllerBase
    {
        private readonly FeedbacksService _feedbacksService;

        public FeedbacksController(FeedbacksService feedbacksService)
        {
            _feedbacksService = feedbacksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
        {
            if (filters.Page < 1)
            {
                throw new FormValidationException("Page must be greater than 0");
            }

            var feedbacks = await _feedbacksService.GetByFilter(filters);
            return Ok(feedbacks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var feedback = await _feedbacksService.GetById(id);
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FeedbackRequest request)
        {
            var response = await _feedbacksService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var response = await _feedbacksService.Update(id, new FeedbackRequest());
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string ids)
        {
            await _feedbacksService.Delete(ids);
            return Ok(new SuccessResponse());
        }
    }
}