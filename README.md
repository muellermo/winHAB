winHAB
======

winHAB a generic app based on openHAB for windows 8 / rt


Currently Bugs
  - The UI gets only on update at runtime. After DataStruct changed, PropertyListener don't recognizes changes.
        - Possibale solution --> disable Cachmode (HttpClient)
