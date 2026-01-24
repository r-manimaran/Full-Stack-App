
/**
 * Builds an Angular application using npm and Angular CLI
 * 
 * @param path The directory path where the Angular project is located
 * 
 * Example usage:
 * angularBuild('./frontend')
 */
def call(String path) {
    // Validate input parameter
    if (!path?.trim()) {
        error "Path parameter cannot be null or empty"
    }
    
    // Install dependencies and build Angular app
    sh """
        set -e
        cd ${path}
        npm install
        ng build --configuration=production
    """
}