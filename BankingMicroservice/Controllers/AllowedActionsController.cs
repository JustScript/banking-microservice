using System.Net.Mime;
using BankingMicroservice.DTO;
using BankingMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankingMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class AllowedActionsController : ControllerBase
{
    private readonly ILogger<AllowedActionsController> _logger;
    private readonly ICardService _cardService;
    private readonly IActionsService _actionsService;

    public AllowedActionsController(
        ILogger<AllowedActionsController> logger,
        ICardService cardService,
        IActionsService actionsService)
    {
        _logger = logger;
        _cardService = cardService;
        _actionsService = actionsService;
    }

    [HttpGet(Name = "AllowedActions")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Get allowed actions for a card",
        Description = "Get allowed actions for a card based on the user and card details")]
    [SwaggerResponse(StatusCodes.Status200OK, "Allowed actions for the card", typeof(object))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or card cannot be found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetAllowedActions([FromQuery] AllowedActionsRequestDto model)
    {
        _logger.LogInformation("Received request to get allowed actions for UserId: {UserId}, CardId: {CardId}", model.UserId, model.CardId);

        if (!model.IsValid)
        {
            _logger.LogWarning("Invalid request received for UserId: {UserId}, CardId: {CardId}", model.UserId, model.CardId);
            return BadRequest("Invalid request");
        }

        var card = await _cardService.GetCardDetails(model.UserId, model.CardId);
        if (card == null)
        {
            _logger.LogWarning("Card not found for UserId: {UserId}, CardId: {CardId}", model.UserId, model.CardId);
            return BadRequest("Card cannot be found");
        }

        var allowedActions = _actionsService.GetAllowedActions(card);
        _logger.LogInformation("Allowed actions retrieved for UserId: {UserId}, CardId: {CardId} - {allowedActions}", model.UserId, model.CardId, allowedActions);

        return Ok(new { allowedActions });
    }
}
