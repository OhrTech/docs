// The Device SDK pages' Kotlin and Java snippets (scripts/check-device-snippets.mjs writes them into src/.../generated).
pluginManagement {
    repositories {
        google()
        mavenCentral()
        gradlePluginPortal()
    }
}
dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.FAIL_ON_PROJECT_REPOS)
    repositories {
        google()
        mavenCentral()
        // OHR_MAVEN_LOCAL: an unreleased net.ohrtech:device-sdk from ~/.m2 (publishToMavenLocal).
        if (System.getenv("OHR_MAVEN_LOCAL") != null) mavenLocal()
    }
}
rootProject.name = "device-snippets"
