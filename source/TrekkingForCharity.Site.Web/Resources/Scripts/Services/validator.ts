import * as validate from 'validate.js'
export class Validator {
    private readonly form: HTMLFormElement;
    private readonly  elements: NodeListOf<Element>;
    private readonly  constraints: any;
    constructor(formQuery: string) {
        this.form = document.querySelector(formQuery) as HTMLFormElement;
        this.constraints = {};
        const contextThis = this;
        this.elements = this.form.querySelectorAll('input:not([type="hidden"]), textarea');
        Array.prototype.forEach.call(this.elements, (element: HTMLInputElement) => {
            let needsValidation: boolean = false;
            let elementName = this.convertNestedObject(element.name);
            contextThis.constraints[elementName] = {};            
            for (let i in element.dataset) {
                element.placeholder = '';
                switch (i) {
                    case 'valRequired':
                        needsValidation = true;
                        contextThis.constraints[elementName].presence = {
                            message: '^' + element.dataset[i]
                        };

                        break;
                    case 'valEmail':
                        needsValidation = true;
                        contextThis.constraints[elementName].email = {
                            message: '^' + element.dataset[i]
                        };
                }
            }
            if (needsValidation) {
                element.addEventListener('blur', (e) => { contextThis.elementChange(e); });
                element.addEventListener('change', (e) => { contextThis.elementChange(e); });
            }
        });
        this.form.addEventListener('submit', (e) => { contextThis.formSubmit(e); });
    }

    private elementChange(event: Event): void {
        this.performValidation();
    }
    private formSubmit(event: Event): void {

        if (this.performValidation())
            event.preventDefault();
    }
    private performValidation(): any {
        let formValues = validate.collectFormValues(this.form);
        var parsedFormValues = {}
        for(var name in formValues) {
            var parsedName = name.replace('\\\\', '\\');
            parsedFormValues[parsedName] = formValues[name];            
        }
        let validationResult = validate(parsedFormValues, this.constraints);
        let contextThis = this;
        Array.prototype.forEach.call(this.elements, (element: HTMLInputElement) => {
            contextThis.decorateElement(element, validationResult);
        });
        return validationResult;
    }
    private decorateElement(element: HTMLInputElement, validationResult: any) {
        const group = element.closest(".field");
        group.classList.remove('is-danger');
        group.classList.remove('is-success');
        element.classList.remove('is-danger');
        element.classList.remove('is-success');        
        let helpblock = group.querySelector('.help');
        helpblock.innerHTML = '';
        helpblock.classList.remove('is-danger');
        helpblock.classList.remove('is-success');
        let elementName = this.convertNestedObject(element.name);
        let item = validationResult[elementName];
        if (item) {            
            element.classList.add('is-danger');
            helpblock.classList.add('is-danger');
            group.classList.add('is-danger');
            for (var failure in item) {
                console.log(item[failure]);
                helpblock.innerHTML = item[failure];
            }
        } else {
            group.classList.add('is-success');
        }
    }
    private convertNestedObject(name:string):string {
        return name.replace('.', '\\.')
    }
}