using System;
using System.DirectoryServices;
using System.Collections;

namespace SCUDocker.INFRASTRUCTURE.SERVICES
{
    public class LdapService
    {
        public void GetDomains(string ldapServer, string adminUser, string adminPassword)
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
                    {
                        // Mostrar todas las propiedades del resultado para ver qué contiene
                        Console.WriteLine("Propiedades del dominio:");

                        foreach (DictionaryEntry property in result.Properties)
                        {
                            // Obtener la propiedad como ResultPropertyValueCollection
                            var values = (ResultPropertyValueCollection)property.Value;

                            // Imprimir cada valor en la colección de propiedades
                            Console.WriteLine($"Propiedad: {property.Key}");
                            foreach (var value in values)
                            {
                                Console.WriteLine($"  Valor: {value}");
                            }
                        }

                        // if (result.Properties.Contains("dn") && result.Properties["dn"].Count > 0)
                        // {
                        //     Console.WriteLine("Dominio encontrado: " + result.Properties["dn"][0]);
                        // }
                        // else
                        // {
                        //     Console.WriteLine("Dominio encontrado sin 'dn'.");
                        // }
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los dominios: {ex.Message}");
            }
        }

    }
}
