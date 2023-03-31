// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Asignar Propietario a un Inmueble
let btnAsignar = document.getElementById('btn-asignar');
let add = document.querySelectorAll('.fa-plus');

btnAsignar.addEventListener('click', () => {
    let add = document.querySelectorAll('.fa-plus');

    add.forEach((btn) => {
        btn.addEventListener('click', (e) => {
            const propietario = document.getElementById(e.target.id)
            const propietarioData = propietario.parentElement.parentElement;

            let label = document.createElement('label');
            label.innerHTML = "Propietario"
            label.for = "PropietarioId";
            label.className = "control-label"

            let input = document.createElement('input');
            input.name = "PropietarioId"
            input.type = "hidden"
            input.value = propietarioData.childNodes[1].innerText;
            input.style.display = 'inline';

            let span = document.createElement('span');
            span.innerText = propietarioData.childNodes[3].innerText;
            span.className = "form-control"
            
            let i = document.createElement('i');
            i.classList.add('fa-solid', 'fa-user-minus');
            i.id = 'reasignarPropietario'
            i.addEventListener('click', () => {
                i.remove();
                select.remove();
                PROPIETARIO_CONTAINER.replaceChild(btnAsignar, label);
            })
            let select = document.createElement('select');
            select.className = "form-control"
            select.name = "PropietarioId"
            select.style.display = 'inline';
            
            label.appendChild(span)
            
            

            const PROPIETARIO_CONTAINER = document.getElementById('propietario-container');
            PROPIETARIO_CONTAINER.replaceChild(label, btnAsignar);
            PROPIETARIO_CONTAINER.appendChild(input);
            PROPIETARIO_CONTAINER.appendChild(i)
            
        })
        
    })
})


