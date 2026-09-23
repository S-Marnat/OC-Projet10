using Microsoft.AspNetCore.Mvc;
using Moq;
using Notes.DTOs;
using Notes.Services.Interfaces;

namespace Notes.Tests.UnitTests.Controllers
{
    public class NoteControllerTests
    {
        private readonly Mock<INoteService> _serviceMock;
        private readonly NoteController _controller;

        public NoteControllerTests()
        {
            _serviceMock = new Mock<INoteService>();
            _controller = new NoteController(_serviceMock.Object);
        }


        // -- GetNoteByPatient --

        [Fact]
        public async Task GetNoteByPatient_AucuneCondition_RetourneOkAvecListeNotesPourPatient1()
        {
            // Arrange
            var notes = new List<NoteReadDto>
            {
                new NoteReadDto
                {
                    Id = "1",
                    Contenu = "Contenu Test 1",
                    DateCreation = new DateTime(2026, 1, 1),
                    IdPatient = 1
                },

                new NoteReadDto
                {
                    Id = "2",
                    Contenu = "Contenu Test 2",
                    DateCreation = new DateTime(2026, 1, 2),
                    IdPatient = 1
                }
            };

            _serviceMock.Setup(s => s.GetByPatientAsync(1))
                .ReturnsAsync(notes);

            // Act
            var result = await _controller.GetNoteByPatient(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsAssignableFrom<IEnumerable<NoteReadDto>>(ok.Value);
            Assert.Equal(2, returnedList.Count());
        }

        [Fact]
        public async Task GetNoteByPatient_AucuneDonnee_RetourneOkAvecListeVide()
        {
            // Arrange
            var notes = new List<NoteReadDto>();
            _serviceMock.Setup(s => s.GetByPatientAsync(1))
                .ReturnsAsync(notes);
            
            // Act
            var result = await _controller.GetNoteByPatient(1);
            
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsAssignableFrom<IEnumerable<NoteReadDto>>(ok.Value);
            Assert.Empty(returnedList);
        }

        [Fact]
        public async Task GetNoteByPatient_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByPatientAsync(1))
                .ThrowsAsync(new Exception("Erreur de test"));
            
            // Act
            var result = await _controller.GetNoteByPatient(1);
            
            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


        // -- GetNoteById --

        [Fact]
        public async Task GetNoteById_IdValide_RetourneOkAvecNote()
        {
            // Arrange
            var notes = new List<NoteReadDto>
            {
                new NoteReadDto
                {
                    Id = "1",
                    Contenu = "Contenu Test 1",
                    DateCreation = new DateTime(2026, 1, 1),
                    IdPatient = 1
                }
            };

            _serviceMock.Setup(s => s.GetByIdAsync("1"))
                .ReturnsAsync(notes.First());

            // Act
            var result = await _controller.GetNoteById("1");

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedNote = Assert.IsType<NoteReadDto>(ok.Value);
            Assert.Equal("Contenu Test 1", returnedNote.Contenu);
        }

        [Fact]
        public async Task GetNoteById_IdInexistant_RetourneNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync("999"))
                .ReturnsAsync((NoteReadDto)null);

            // Act
            var result = await _controller.GetNoteById("999");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("L'Id renseigné ne correspond à aucune note.", notFound.Value);
        }

        [Fact]
        public async Task GetNote_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _controller.GetNoteById("1");

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


        // -- PostNote --

        [Fact]
        public async Task PostNote_DonneesValides_RetourneCreatedAvecNote()
        {
            // Arrange
            var newNote = new NoteCreateDto
            {
                Contenu = "Contenu Test 1",
                IdPatient = 1
            };

            var createdNote = new NoteReadDto
            {
                Id = "1",
                Contenu = "Contenu Test 1",
                DateCreation = new DateTime(2026, 1, 1),
                IdPatient = 1
            };

            _serviceMock.Setup(s => s.CreateAsync(newNote))
                .ReturnsAsync(createdNote);

            // Act
            var result = await _controller.PostNote(newNote);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedNote = Assert.IsType<NoteReadDto>(created.Value);
            Assert.Equal("1", returnedNote.Id);
        }

        [Fact]
        public async Task PostNote_ModelStateInvalide_RetourneBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Contenu", "Required");

            // Act
            var result = await _controller.PostNote(new NoteCreateDto());

            // Assert
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<NoteCreateDto>()), Times.Never);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Les informations fournies pour la note sont invalides.", badRequest.Value);
        }

        [Fact]
        public async Task PostNote_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<NoteCreateDto>()))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _controller.PostNote(new NoteCreateDto());

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


        // -- PutNote --

        [Fact]
        public async Task PutNote_IdCorrespond_RetourneOkAvecNote()
        {
            // Arrange
            var note = new NoteUpdateDto
            {
                Id = "1",
                Contenu = "Contenu Test 1"
            };

            var updatedNote = new NoteReadDto
            {
                Id = "1",
                Contenu = "Contenu Test 1",
                DateCreation = new DateTime(2026, 1, 1),
                IdPatient = 1
            };

            _serviceMock.Setup(s => s.UpdateAsync("1", note))
                .ReturnsAsync(updatedNote);

            // Act
            var result = await _controller.PutNote("1", note);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var returnedNote = Assert.IsType<NoteReadDto>(ok.Value);
            Assert.Equal("1", returnedNote.Id);
        }

        [Fact]
        public async Task PutNote_IdInexistant_RetourneNotFound()
        {
            // Arrange
            var note = new NoteUpdateDto
            {
                Id = "1",
                Contenu = "Contenu Test 1"
            };

            var updatedNote = new NoteReadDto
            {
                Id = "1",
                Contenu = "Contenu Test 1",
                DateCreation = new DateTime(2026, 1, 1),
                IdPatient = 1
            };

            // Act
            var result = await _controller.PutNote("99", note);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("L'Id renseigné ne correspond à aucune note.", notFound.Value);
        }

        [Fact]
        public async Task PutNote_ModelStateInvalide_RetourneBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Contenu", "Required");

            // Act
            var result = await _controller.PutNote("1", new NoteUpdateDto());

            // Assert
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<NoteUpdateDto>()), Times.Never);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Les informations fournies pour la note Id=1 sont invalides.", badRequest.Value);
        }

        [Fact]
        public async Task PutNote_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<NoteUpdateDto>()))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _controller.PutNote("1", new NoteUpdateDto());

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


        // -- DeleteNote --

        [Fact]
        public async Task DeleteNote_IdValide_RetourneNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync("1")).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteNote("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteNote_IdInexistant_RetourneNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync("999"))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteNote("999");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("L'Id renseigné ne correspond à aucune note.", notFound.Value);
        }

        [Fact]
        public async Task DeleteNote_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _controller.DeleteNote("1");

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }
    }
}
