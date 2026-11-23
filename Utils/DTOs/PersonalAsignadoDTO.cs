using System.Collections.Generic;

namespace Taller.Utils.DTOs;

public class PersonalAsignadoDTO {
        
        public string Dni;
        public string Nombre;
        public string Email;
        public List<SeleccionReparacionDTO> ReparacionesAsignadas;
        public PersonalAsignadoDTO(){}
    }