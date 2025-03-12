using System;
using System.DirectoryServices;
using System.Collections;
using System.Runtime.InteropServices;

namespace SCUDocker.INFRASTRUCTURE.SERVICES
{
    public class LdapService
    {
        public void GetDomains(string ldapServer, string adminUser, string adminPassword)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    // Crear la conexión LDAP utilizando la URL del servidor
                    string ldapPath = $"LDAP://{ldapServer}";
                    DirectoryEntry entry = new DirectoryEntry(ldapPath, adminUser, adminPassword);
                    // Crear el objeto de búsqueda
                    DirectorySearcher searcher = new DirectorySearcher(entry)
                    {
                        // Filtro de búsqueda de dominios
                        Filter = "(objectClass=domainDNS)", // Cambiar el filtro según lo que necesites (dominios)
                        SearchScope = SearchScope.Subtree // Asegúrate de buscar en todo el árbol
                    };
                    // Realizar la búsqueda
                    SearchResultCollection results = searcher.FindAll();
                    // Verificar si se encontraron resultados
                    if (results.Count == 0)
                    {
                        Console.WriteLine("No se encontraron dominios.");
                    }
                    else
                    {
                        foreach (SearchResult result in results)
                        {// Mostrar todas las propiedades del resultado para ver qué contiene
                            Console.WriteLine("Propiedades del dominio:");

                            foreach (DictionaryEntry property in result.Properties)
                            {
                                // Obtener la propiedad como ResultPropertyValueCollection
                                if (property.Value != null)
                                {
                                    var values = (ResultPropertyValueCollection)property.Value;
                                    // Imprimir cada valor en la colección de propiedades
                                    Console.WriteLine($"Propiedad: {property.Key}");
                                    foreach (var value in values)
                                    {
                                        Console.WriteLine($"  Valor: {value}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Propiedad: {property.Key} es nula");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener los dominios: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("LDAP functionality is only supported on Windows.");
            }
        }


        public void GetUserProfiles(string ldapServer, string adminUser, string adminPassword)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    // Crear la conexión LDAP utilizando la URL del servidor
                    string ldapPath = $"LDAP://{ldapServer}";
                    DirectoryEntry entry = new DirectoryEntry(ldapPath, adminUser, adminPassword);

                    // Crear el objeto de búsqueda
                    DirectorySearcher searcher = new DirectorySearcher(entry)
                    {
                        // Filtro para obtener solo objetos de tipo "usuario"
                        Filter = "(objectClass=user)",
                        SearchScope = SearchScope.Subtree // Asegurar que busca en todo el árbol de AD
                    };

                    // Especificar las propiedades a recuperar
                    searcher.PropertiesToLoad.Add("cn");               // Nombre común del usuario
                    searcher.PropertiesToLoad.Add("samAccountName");   // Nombre de usuario en el dominio
                    searcher.PropertiesToLoad.Add("mail");             // Correo electrónico
                    searcher.PropertiesToLoad.Add("memberOf");         // Grupos a los que pertenece el usuario

                    // Ejecutar la búsqueda
                    SearchResultCollection results = searcher.FindAll();

                    // Verificar si se encontraron usuarios
                    if (results.Count == 0)
                    {
                        Console.WriteLine("No se encontraron perfiles de usuario.");
                    }
                    else
                    {
                        Console.WriteLine("Perfiles de usuario encontrados:");

                        foreach (SearchResult result in results)
                        {
                            Console.WriteLine("Usuario:");

                            foreach (DictionaryEntry property in result.Properties)
                            {
                                if (property.Value != null)
                                {
                                    var values = (ResultPropertyValueCollection)property.Value;
                                    Console.WriteLine($"  Propiedad: {property.Key}");

                                    foreach (var value in values)
                                    {
                                        Console.WriteLine($"    Valor: {value}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"  Propiedad: {property.Key} es nula");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener los perfiles de usuario: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("LDAP functionality is only supported on Windows.");
            }
        }

        public List<Dictionary<string, object>> GetAllGroupsList(string ldapServer, string adminUser, string adminPassword)
{
    List<Dictionary<string, object>> groupsList = new List<Dictionary<string, object>>();

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        try
        {
            // Crear la conexión LDAP utilizando la URL del servidor
            string ldapPath = $"LDAP://{ldapServer}";
            DirectoryEntry entry = new DirectoryEntry(ldapPath, adminUser, adminPassword);

            // Crear el objeto de búsqueda
            DirectorySearcher searcher = new DirectorySearcher(entry)
            {
                // Filtro para buscar todos los grupos (objectClass=group)
                Filter = "(objectClass=group)",
                SearchScope = SearchScope.Subtree
            };

            // Especificar las propiedades que deseamos recuperar
            searcher.PropertiesToLoad.Add("cn"); // Nombre común del grupo
            searcher.PropertiesToLoad.Add("distinguishedName"); // Nombre distinguido del grupo

            // Ejecutar la búsqueda
            SearchResultCollection results = searcher.FindAll();

            // Verificar si se encontraron resultados
            if (results.Count == 0)
            {
                Console.WriteLine("No se encontraron grupos.");
            }
            else
            {
                // Iterar sobre los resultados y agregar los grupos a la lista
                foreach (SearchResult result in results)
                {
                    Dictionary<string, object> groupProfile = new Dictionary<string, object>();

                    foreach (DictionaryEntry property in result.Properties)
                    {
                        if (property.Value != null && property.Key != null)
                        {
                            var values = (ResultPropertyValueCollection)property.Value;
                            groupProfile[property.Key?.ToString() ?? string.Empty] = values.Cast<object>().ToList();
                        }
                        else if (property.Key != null)
                        {
                            groupProfile[property.Key?.ToString() ?? string.Empty] = null;
                        }
                    }

                    groupsList.Add(groupProfile);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener los grupos: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("LDAP functionality is only supported on Windows.");
    }

    return groupsList;
}




    }
}
