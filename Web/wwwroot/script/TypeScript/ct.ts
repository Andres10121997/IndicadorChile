/**
 * @access public
 * @returns {void}
 * @description Repositorio: https://github.com/csswizardry/ct
 */
(function (): void {
    /**
     * @constant
     * @type {HTMLLinkElement}
     * @name ct
     */
    const ct: HTMLLinkElement = document.createElement('link');

    ct.rel = 'stylesheet';
    ct.href = 'https://csswizardry.com/ct/ct.css';
    ct.classList.add('ct');

    document.head.appendChild(ct);
}());