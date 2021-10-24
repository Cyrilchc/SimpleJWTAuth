# SimpleJWTAuth

Projet à réutiliser si une authentification simple est à mettre en place

Pour me rappeler : elle montre de manière basique les éléments suivants :

* Comment protéger une api en demandant un jwt
* Comment fournir des jwt
* Comment enregistrer un utilisateur TODO => Voir si l'enregistrement du sel est une bonne pratique
* Comment authentifier un individu 
    * Vérification des entrées (Mail, mdp)
    * Comparaison du hash...
* Le chiffrement de mot de passe
* L'utilisation d'orm (dapper)


## TODO
- Voir comment gérer le rafraichissement de jeton
- éventuellement s'assurer que le jeton reçu a été précédemment fabriqué par mon service pour éviter les fraudes : https://stackoverflow.com/a/30903398/10506880