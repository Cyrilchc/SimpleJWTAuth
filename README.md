# SimpleJWTAuth

Projet à réutiliser si un authentification simple est à mettre en place

Pour me rappeler : elle montre de manière basique les concepts suivants :

* Comment protéger une api en demandant un jwt
* Comment fournir des jwt
* Comment enregistrer un utilisateur TODO => Voir si l'enregistrement du sel est une bonne pratique
* Comment authentifier un individu 
    * Vérification des entrées (Mail, mdp)
    * Comparaison du hash...
* Pratique d'orm 


## TODO
- Voir comment gérer le rafraichissement de jeton
- éventuellement s'assurer que le jeton reçu a été précédemment fabriqué par mon service pour éviter les fraudes : https://stackoverflow.com/a/30903398/10506880