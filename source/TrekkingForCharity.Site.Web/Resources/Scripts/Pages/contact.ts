// import { Validator } from '../Services/validator'

// class Contact {
//     private contactValidator:Validator;

//     constructor() {
//         if (document.readyState !== 'loading') {
//             this.init();
//         } else {
//             document.addEventListener('DOMContentLoaded', (e) => { this.init(); });
//         }
//     }
//     init() {
//         this.contactValidator = new Validator('#contact-form');
//     }
// }

// let page: Contact = new Contact();

import Vue from 'vue'
const MyComponent = new Vue({
    el: '#app',
    data: {
      message: 'Hello Vue!'
    },
    created: function () {
        // `this` points to the vm instance
        console.log('a is: ' + this.message)
      }
});
