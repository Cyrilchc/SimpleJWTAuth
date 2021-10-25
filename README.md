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
* Vérification de l'authenticité du jeton (Vérifie que le jeton reçu a été délivré par mon système - Source : https://stackoverflow.com/a/30903398/10506880)


## TODO
- Ajouter le refresh token