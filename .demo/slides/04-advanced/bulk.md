---
theme: default
layout: section
---

# Bulk Publishing

```mermaid
graph LR
    S[Sender]
    subgraph Broker
        T@{ shape: das, label: "Topic" }
    end
    R[Receiver1]
    S -.->|"[Messages]"| T
    T -.->|"Message"| R
     T -.->|"Message"| R
      T -.->|"Message"| R
```

---

```mermaid
graph LR
    S[Sender]
    subgraph Broker
        T@{ shape: das, label: "Topic" }
    end
    R[Receiver1]
    S -.->|"[Messages]"| T
    T -.->|"[Messages]"| R
```