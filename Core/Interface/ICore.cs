using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
    public interface ICore<T>
    {
         T Cadastrar();
        T Editar();
        T Deletar(string id);
        T ListarPorId(string id);
        T ListarTodos();

    }
}
