using System.Collections.Generic;

namespace Taller.Utils.DTOs;

public class PersonalAsignadoDTO {
        
        public string Dni = string.Empty;
        public string Nombre = string.Empty;
        public string Email = string.Empty;
        public List<string> ReparacionesAsignadas = [];
        public PersonalAsignadoDTO(){}
    }