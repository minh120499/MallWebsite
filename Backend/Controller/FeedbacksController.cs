using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FeedbackRequest request)
    {
        var response = await _feedbacksService.Create(request);
        return Ok(response);
    }
}