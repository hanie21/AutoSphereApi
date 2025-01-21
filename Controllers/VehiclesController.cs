using AutoSphere.Api.Model;
using Microsoft.AspNetCore.Mvc;
using AutoSphere.Api.Services;
using Microsoft.Extensions.Caching.Memory;


namespace AutoSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ISavedSearchService _savedSearchService;
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IMemoryCache _cache; 

        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(
                    IVehicleService vehicleService,  
                    ISavedSearchService savedSearchService, 
                    IVehicleSearchService vehicleSearchService, 
                    ILogger<VehiclesController> logger,
                    IMemoryCache cache)
        {
            _vehicleService = vehicleService;
            _savedSearchService = savedSearchService;
            _vehicleSearchService = vehicleSearchService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchVehicleRequest request)
        {
            _logger.LogInformation("Search method called with parameters: {@Request}", request);

            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                _logger.LogWarning("Invalid parameters: PageNumber={PageNumber}, PageSize={PageSize}. Must be greater than zero.", request.PageNumber, request.PageSize);
                return BadRequest("PageNumber and PageSize must be greater than zero.");
            }

            // Define a cache key based on the request parameters
            string cacheKey = $"search_results_{request.PageNumber}_{request.PageSize}_{request.Make}_{request.Model}";

            // Check if the data is available in cache
            if (!_cache.TryGetValue(cacheKey, out var cachedResults))
            {
                // If not in cache, fetch data from the service (database or external API)
                _logger.LogInformation("Cache miss: Fetching search results from the service.");
                var results = await _vehicleService.SearchVehiclesAsync(request);

                // Store the results in the cache for future use (expires in 10 minutes)
                _cache.Set(cacheKey, results, TimeSpan.FromMinutes(10));
                _logger.LogInformation("Search completed successfully. {ResultCount} results found and cached.", results.Count());

                return Ok(results);
            }

            // If data is in cache, return cached results
            _logger.LogInformation("Returning cached search results.");
            return Ok(cachedResults);
        }

        [HttpGet("open-search")]
        public async Task<IActionResult> SearchVehicles(string query)
        {
            var results = await _vehicleSearchService.SearchVehiclesAsync("vehicles", query);
            return Ok(results);
        }

        [HttpPost("index-open-search")]
        public async Task<IActionResult> IndexTo([FromBody] AddVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _vehicleSearchService.IndexVehicleAsync("vehicles", request);
            return Ok(vehicle);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddVehicleRequest request)
        {
            _logger.LogInformation("Add vehicle request received: {@Request}", request);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var vehicle = await _vehicleService.AddVehicleAsync(request);
            _logger.LogInformation("Vehicle added successfully with ID={VehicleId}.", vehicle.Id);
            return CreatedAtAction(nameof(Search), new { vehicle.Id }, vehicle);
        }

        [HttpPost("save-search")]
        public async Task<IActionResult> SaveSearch([FromBody] SavedSearchRequest request)
        {
            _logger.LogInformation("Save search request received: {@Request}", request);

            var savedSearch = await _savedSearchService.SaveSearchAsync(request);
            _logger.LogInformation("Search saved successfully with ID={SavedSearchId}.", savedSearch.Id);
            return Ok(savedSearch);
        }

        [HttpGet("saved-searches/{userId}")]
        public async Task<IActionResult> GetSavedSearches(int userId)
        {
            var savedSearches = await _savedSearchService.GetSavedSearchesAsync(userId);
            return Ok(savedSearches);
        }
    
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new InvalidOperationException("This is a test exception.");
        }

        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            _logger.LogInformation("This is an informational message.");
            _logger.LogError("This is an error message with additional context: {Context}", "SampleContext");

            return Ok("Logged successfully.");
        }

        [HttpGet("fuzzy-search")]
        public async Task<IActionResult> FuzzySearch(string query)
        {
            var results = await _vehicleSearchService.FuzzyMatchingSearchVehicleAsync("vehicles", query);
            return Ok(results);
        }
    }
}

