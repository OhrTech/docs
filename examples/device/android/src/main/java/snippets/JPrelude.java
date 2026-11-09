package snippets;

import android.app.Activity;
import android.content.Context;
import java.util.concurrent.Executor;

/** The names a page's text supplies around a Java fragment. */
@SuppressWarnings("unused")
public class JPrelude {
    protected Activity activity;
    protected Context context;
    protected Executor executor = Runnable::run;
    protected byte[] serverNonce = new byte[32];
    protected byte[] challenge = new byte[32];
    protected byte[] ciphertext = new byte[1120];
    protected byte[] enc = new byte[32];
    protected String ED25519_PUBLIC_KEY = "";
    protected void show(Object... values) {}
    protected void sendToServer(Object... values) {}
}
