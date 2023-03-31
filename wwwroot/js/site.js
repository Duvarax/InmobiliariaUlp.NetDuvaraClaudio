// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Asignar Propietario a un Inmueble
let btnAsignar = document.getElementById('btn-asignar');
let containerPropietario = document.getElementById('propietario-container')
if(btnAsignar){
btnAsignar.addEventListener('click', seleccion("propietario", btnAsignar, containerPropietario));
}

//Only
let btnAsignarInquilino = document.getElementById('btn-asignar-inquilino')
let containerInquilino = document.getElementById('inquilino-container')
if(btnAsignarInquilino){
    btnAsignarInquilino.addEventListener('click', seleccion("inquilino", btnAsignarInquilino, containerInquilino));
}

let btnAsignarInmueble = document.getElementById('btn-asignar-inmueble')
let containerInmueble = document.getElementById('propietario-container')
if(btnAsignarInmueble){
btnAsignarInmueble.addEventListener('click', seleccion("inmueble", btnAsignarInmueble,containerInmueble));
}
//Contrato


const reasignarBtns = document.querySelectorAll('.fa-user-minus');
if(reasignarBtns){
    
    reasignarBtns.forEach((btn) => {
        btn.addEventListener('click', (e) => {
            let contenedor = e.target.parentNode;
            contenedor.innerHTML = "";

            let button = document.createElement('h1');
            button.id = `btn-asignar-${contenedor.id.split('-')[0]}`
            button.classList.add('btn', 'btn-info')
            button.addEventListener('click', ()=>{
                let nombre = contenedor.id.split('-')[0]
                $(`#modal${nombre}`).modal('show');
                btnAsignarInquilino = document.getElementById('btn-asignar-inquilino')
                if(btnAsignarInquilino){
                btnAsignarInquilino.addEventListener('click', seleccion(`${contenedor.id.split('-')[0]}`, btnAsignarInquilino, contenedor))
                }
                let btnAsignarInmueble = document.getElementById('btn-asignar-inmueble')
                if(btnAsignarInmueble){
                btnAsignarInmueble.addEventListener('click', seleccion(`${contenedor.id.split('-')[0]}`, btnAsignarInmueble, contenedor))
                }
            })
            button.innerText = `Asignar ${contenedor.id.split('-')[0]}`

            contenedor.appendChild(button);
        })
    })
}

function seleccion(clase, btnAsignado, container){
    let add = document.querySelectorAll('.fa-plus');

    add.forEach((btn) => {
        btn.addEventListener('click', (e) => {
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
                select.remove();
                PROPIETARIO_CONTAINER.replaceChild(btnAsignado, label);
            })
            let select = document.createElement('select');
            select.className = "form-control"
            select.name = `${clase}Id`;
            select.style.display = 'inline';
            
            label.appendChild(span)
            
            

            const PROPIETARIO_CONTAINER = container;
            PROPIETARIO_CONTAINER.replaceChild(label, btnAsignado);
            PROPIETARIO_CONTAINER.appendChild(input);
            PROPIETARIO_CONTAINER.appendChild(i)
            
        })
        
    })
}