package snippets

import android.app.Activity
import android.content.Context
import java.util.concurrent.Executor

/** The names a page's text supplies around a Kotlin fragment: your activity, your server's values, ... */
@Suppress("unused")
open class Prelude {
    lateinit var activity: Activity
    val context: Context get() = activity
    val executor: Executor = Executor { it.run() }
    val serverNonce = ByteArray(32)
    val serverChallenge = ByteArray(32)
    val challenge = ByteArray(32)
    val ciphertext = ByteArray(1120)
    val enc = ByteArray(32)
    val ED25519_PUBLIC_KEY = ""
    fun reload(vararg values: Any?) {}
    fun show(vararg values: Any?) {}
    fun sendToServer(vararg values: Any?) {}
}
