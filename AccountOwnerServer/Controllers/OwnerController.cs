using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository,IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();
                _logger.LogInfo($"Returned all owners from database");

                return Ok(owners);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned owner with id: {id}");

                var ownerResult = _mapper.Map<OwnerDto>(owner);

                return Ok(ownerResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
