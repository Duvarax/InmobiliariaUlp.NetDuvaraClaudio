// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Asignar Propietario a un Inmueble

document.addEventListener('DOMContentLoaded', () => {
    let btn_asignar_propietario = document.getElementById('btn-asignar-propietario');
        if(btn_asignar_propietario){
            btn_asignar_propietario.addEventListener('click', seleccion("propietario", btn_asignar_propietario, btn_asignar_propietario.parentNode));
        }
    
        let btn_asignar_inquilino = document.getElementById('btn-asignar-inquilino');
        if(btn_asignar_inquilino){
            btn_asignar_inquilino.addEventListener('click', seleccion("inquilino", btn_asignar_inquilino, btn_asignar_inquilino.parentNode))
        }
    
        let btn_asignar_inmueble = document.getElementById('btn-asignar-inmueble');
        if(btn_asignar_inmueble){
            btn_asignar_inmueble.addEventListener('click', seleccion("inmueble", btn_asignar_inmueble, btn_asignar_inmueble.parentNode))
        }

        let btn_asignar_contrato = document.getElementById('btn-asignar-contrato');
        if(btn_asignar_contrato){
            btn_asignar_contrato.addEventListener('click', seleccion("contrato", btn_asignar_contrato, btn_asignar_contrato.parentNode))
        }
    
        
        let propietario_input = document.querySelector('#propietario-container #PropietarioId');
        if(propietario_input){
            propietario_input.addEventListener('click' , reemplazarSeleccion('propietario', propietario_input))
        }

        let inquilino_input = document.querySelector('#inquilino-container #InquilinoId');
        if(inquilino_input){
            inquilino_input.addEventListener('click', reemplazarSeleccion('inquilino', inquilino_input))
        }
    
        let inmueble_input = document.querySelector('#inmueble-container #InmuebleId');
        if(inmueble_input){
            inmueble_input.addEventListener('click', reemplazarSeleccion('inmueble', inmueble_input))
        }
    
        function reemplazarSeleccion(clase, input){
            let container = document.getElementById(`${clase}-container`)
            console.log(container)
            input.nextElementSibling.addEventListener('click', () => {
                input.parentNode.innerHTML = "";
                let button = document.createElement('p');
                button.id = `btn-asignar-${clase}`
                button.classList.add('btn', 'btn-primary')
                button.innerText = "Asignar Inquilino"
                clase = clase.charAt(0).toUpperCase() + clase.slice(1);
                console.log(clase)
                button.addEventListener('click' , () => {
                    console.log("aa")
                    $(`#Modal${clase}`).modal('show');
                    let btn_asignar_inquilino = document.getElementById('btn-asignar-inquilino');
                    if(btn_asignar_inquilino){
                        btn_asignar_inquilino.addEventListener('click', seleccion("inquilino", btn_asignar_inquilino, btn_asignar_inquilino.parentNode))
                    }
    
                    let btn_asignar_inmueble = document.getElementById('btn-asignar-inmueble');
                    if(btn_asignar_inmueble){
                        btn_asignar_inmueble.addEventListener('click', seleccion("inmueble", btn_asignar_inmueble, btn_asignar_inmueble.parentNode))
                    }
                    let btn_asignar_propietario = document.getElementById('btn-asignar-propietario');
                    if(btn_asignar_propietario){
                        btn_asignar_propietario.addEventListener('click', seleccion("propietario", btn_asignar_propietario, btn_asignar_propietario.parentNode))
                    }
                })
                container.appendChild(button)
            })
            
        }
        
        
        function seleccion(clase, btnAsignado, container){
            let add = document.querySelectorAll(`.fa-${clase}`);
            add.forEach((btn) => {
                btn.addEventListener('click', (e) => {
                    console.log(e.target)
                    const objeto = document.getElementById(e.target.id)
                    const objetoData = objeto.parentElement.parentElement;
                    clase = clase.charAt(0).toUpperCase() + clase.slice(1);
                    let label = document.createElement('label');
                    label.innerHTML = `${clase}`
                    label.for = `${clase}Id`;
                    label.className = "control-label"
        
                    let input = document.createElement('input');
                    input.name = `${clase}Id`;
                    input.type = "hidden"
                    input.value = objetoData.childNodes[1].innerText;
                    input.style.display = 'inline';
        
                    let span = document.createElement('span');
                    span.innerText = objetoData.childNodes[3].innerText;
                    span.className = "form-control"
                    
                    let i = document.createElement('i');
                    i.classList.add('fa-solid', 'fa-user-minus');
                    i.id = `reasignar${clase}`
                    i.addEventListener('click', () => {
                        i.remove();
                        PROPIETARIO_CONTAINER.replaceChild(btnAsignado, label);
                    })
                 
                    label.appendChild(span)
                    
                    
        
                    const PROPIETARIO_CONTAINER = container;
                    PROPIETARIO_CONTAINER.replaceChild(label, btnAsignado);
                    PROPIETARIO_CONTAINER.appendChild(input);
                    PROPIETARIO_CONTAINER.appendChild(i)
                    
                })
                
            })
        }
})