---
title: Remote ADB
description: Run adb against a device anywhere on the internet, end-to-end encrypted, for your engineers.
---

Remote ADB gives your engineers the Android Debug Bridge on a device wherever it is: in a store, in
a vehicle, on mobile data. They use their usual `adb` and Android Studio; the device needs no open
port and no VPN.

## With the `ohr` command line tool

The `ohr` tool signs your engineer in, opens the session, and runs the tunnel:

```sh
ohr login                          # once: approve in the OhrTech Portal
ohr devices                        # find the device
ohr adb connect A0000A00042        # tunnel open until Ctrl-C; adb and Android Studio see the device
ohr shell A0000A00042 getprop ro.build.fingerprint
ohr adb A0000A00042 -- logcat -d   # any one adb command
```

A device is named by its ID, IMEI or serial number.

## How it stays private

- **End-to-end encrypted.** adb traffic is compressed and encrypted on the device and only decrypted
  by the engineer's `ohr` tool. OhrTech's relay passes it along without being able to read it.
- **The device is verified.** The tool checks the device's handshake against the device's attested
  key before sending anything.
- **One engineer's key.** The device's adbd accepts only the ADB key of the engineer who opened the
  session, and only on that session.
- **Permissioned and recorded.** Opening a session needs `RemoteAdb` on the device. Every session is
  recorded: when it opened, how it ended and how much data it carried.

## The API calls

The `ohr` tool uses these, and you can use them to show and end sessions in your own tools:

| Call | |
|---|---|
| `GET /v1/devices/{deviceId}/adb-sessions` | The device's sessions: when, status, how they ended, `bytesUp` and `bytesDown` |
| `POST /v1/devices/{deviceId}/adb-sessions` | Open one (the `ohr` tool does this, with its keys) |
| `GET /v1/adb-sessions/{sessionId}` | One session |
| `DELETE /v1/adb-sessions/{sessionId}` | End it |

A session lasts at most 12 hours. `status` is `pending`, `device-connected`, `active`, `closed` or
`failed`. Every call needs `RemoteAdb` on the device.

The device must have remote ADB allowed: the `remote_adb.enabled` [setting](/management-api/guides/settings/).
