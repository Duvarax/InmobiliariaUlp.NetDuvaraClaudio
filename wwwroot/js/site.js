// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let btnAsignar = document.getElementById('btn-asignar');
let add = document.querySelectorAll('.fa-plus');

btnAsignar.addEventListener('click', () => {
    let add = document.querySelectorAll('.fa-plus');

    add.forEach((btn) => {
        btn.addEventListener('click', (e) => {
            const propietario = document.getElementById(e.target.id)
            const propietarioData = propietario.parentElement.parentElement;
            console.log(propietarioData.childNodes[1]);
            let label = document.createElement('label');
            label.for = "PropietarioId";
            label.className = "control-label"
            let input = document.createElement('option');
            input.name = "PropietarioId"
            input.type = "text"
            input.className = "form-control"
            input.value = propietarioData.childNodes[1].innerText;
            input.innerText = propietarioData.childNodes[3].innerText;
            input.readOnly = true;
            input.style.display = 'inline';

            
            let i = document.createElement('i');
            i.classList.add('fa-solid', 'fa-user-minus');
            i.id = 'reasignarPropietario'
            i.addEventListener('click', () => {
                i.remove();
                PROPIETARIO_CONTAINER.replaceChild(btnAsignar, label);
            })
            

            const PROPIETARIO_CONTAINER = document.getElementById('propietario-container');
            PROPIETARIO_CONTAINER.replaceChild(label, btnAsignar);
            PROPIETARIO_CONTAINER.appendChild(input);
            PROPIETARIO_CONTAINER.appendChild(i);
        })
        
    })
})


