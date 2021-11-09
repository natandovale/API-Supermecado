using System;
using Xunit;
using NSubstitute;
using WebApplication2.Interfaces;
using WebApplication2.HandlersMediator.Request;
using WebApplication2.Commands.Response;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using WebApplication2.Commands.Request;
using WebApplication2.Model;

namespace TesteAPI
{
    public class TesteDeleteProduct
    {
        public readonly IProductRepository _productRepository;
        public readonly DeleteProductHandler _sut;

        public TesteDeleteProduct()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _sut = new DeleteProductHandler(_productRepository);
        }

        [Fact]
        public async Task DeletarProdutoQueNaoExisteDeveRetornarExcessao()
        {
            //Arrange
            Product produto = null;
            DeleteProductRequest request = new DeleteProductRequest() { Id = 1 };
            _productRepository.Get(request.Id).Returns(produto);

            //Act
            Func<Task> act = () => _sut.Handle(request, default);

            //Assert
            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeletarProdutoExistenteDeveRetornarOIdDoProduto()
        {
            //Arrange
            Product produto = new Product() { Id = 1 };
            DeleteProductRequest request = new DeleteProductRequest() { Id = produto.Id };
            _productRepository.Get(request.Id).Returns(produto);

            //Act
            var response = await _sut.Handle(request, default);

            //Assert
            response.Id.Should().Be(produto.Id);
        }

        [Fact]
        public async Task DeletarProdutoDeveChamarODeletarDoRepositorio()
        {
            //Arrange
            Product produto = new Product() { Id = 1 };
            DeleteProductRequest request = new DeleteProductRequest() { Id = produto.Id };
            _productRepository.Get(request.Id).Returns(produto);

            //Act
            await _sut.Handle(request, default);

            //Assert
            _productRepository.Received().Delete(produto.Id);
        }
    }
}
