using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPreguntasService<T> 
        where T : class 
    {
        Task<IEnumerable<object>> GetTeacherPreguntas(int teacherId);
        Task<IEnumerable<object>> GetTeacherPreguntasByTitle(int teacherId, string title);
        Task<IEnumerable<T>> GetPruebasDiagnosticasPreguntas(int pruebasDiagnosticaId);
        Task SavePreguntas(T pregunta, int teacherId);
    }
}
