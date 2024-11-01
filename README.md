# Github Users Search App 
![Screenshot 2024-11-01 at 15 51 32](https://github.com/user-attachments/assets/2aae56c2-0017-464c-ba50-5ab84c486a06)


This project requires a GitHub token to authenticate API requests. If you don't have one, you can create it by following [GitHub's instructions](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

Start this project by running

- **On Windows (Command Prompt):**
     ```cmd
     setx GITHUB_TOKEN <your_github_token_here>
     ```

- **On macOS/Linux (Bash):**
  ```bash
  export GITHUB_TOKEN=<your_github_token_here>
  ```
and then:
```
npm ci
npm start
```

Then visit `http://localhost:3000/`
