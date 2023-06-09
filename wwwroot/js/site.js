﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
                button.innerText = `Asignar ${clase}`
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


        let avatar_input = document.getElementById('avatar_input');
        if(avatar_input){
            const avatar_preview = document.getElementById('avatar_img');
            avatar_input.addEventListener('change', e => {
                    const file = avatar_input.files[0];
                    const reader = new FileReader();
                    reader.addEventListener('load', () => {
                        const imgData = reader.result;
                        avatar_preview.src = imgData;
                    })
                    reader.readAsDataURL(file);
            })
        }

        let rolInput = document.getElementById("RolSelect")
        if(rolInput){
            const HIDDEN_ROL_INPUT = document.getElementById('Rol');
            
            rolInput.addEventListener('click', (e) => {
                HIDDEN_ROL_INPUT.value = rolInput.value;
                HIDDEN_ROL_INPUT.innerText = rolInput.value;
            })
        }

        let btn_show = document.getElementById('show_password')
        if(btn_show){
            const passwordInputs = document.querySelectorAll('.password');

            btn_show.addEventListener('change', function() {
            if (btn_show.checked) {
                passwordInputs.forEach(input => {
                input.type = 'text';
            });
            } else {
                passwordInputs.forEach(input => {
                input.type = 'password';
            });
            }
            });
        } 

        let seleccionar_propietario = document.getElementById('seleccion_propietario');
        if(seleccionar_propietario){
           
        }
        
        
        
        
})

let form_login = document.getElementById('form_login')
        if(form_login){
            const EXPRESION_EMAIL = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            let email_input = document.getElementById('Email')
            let password_input = document.getElementById('Contraseña')
            let submit_btn = document.getElementById('form_submit_btn')
            
            email_input.addEventListener('keyup', () => {
                if(!EXPRESION_EMAIL.test(email_input.value)){
                    submit_btn.disabled = true;
                }else{
                    submit_btn.disabled = false;
                }
            })

        

        }
