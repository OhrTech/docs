package snippets;

import android.app.Activity;
import android.content.Context;
import java.util.concurrent.Executor;
import net.ohrtech.device.OhrWifiView;

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
    protected String MLDSA65_PUBLIC_KEY = "";
    protected byte[] theirPublicKey = new byte[1184];
    protected OhrWifiView wifiView;
    protected android.view.View updateBanner;
    protected String staffPassword = "";
    protected void applyTheme(boolean dark) {}
    protected void reload(Object... values) {}
    protected void show(Object... values) {}
    protected void sendToServer(Object... values) {}
    protected MyServer myServer = new MyServer();

    /** Your own server's API, as the attestation page calls it. */
    protected static class MyServer {
        public String newChallenge() { return ""; }
        public void register(String json) {}
    }
}
