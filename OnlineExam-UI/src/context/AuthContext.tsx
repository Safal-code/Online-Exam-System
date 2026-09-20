//any component can access user without passing it through 5 components
import {
  createContext, //creates global storage/access system
  useEffect,
  useState,
  type ReactNode, //tell typescript what children can contain
} from "react";

import type { User } from "../types/user";

interface AuthContextType {
  user: User | null;
  login: (user: User) => void;
  logout: () => void;
}

//it creates actual context global box/storage so it can pass who is logged in directly to component like(dashboard,navbar,profile etc.)
export const AuthContext = createContext<AuthContextType | undefined>(  ////explain auth context in simple words line by line still didnt get what and why
  undefined   //initially undefined
);


//"I want to create a component called AuthProvider, and it can contain other React components inside it."
//
interface AuthProviderProps {
  children: ReactNode;
}

//provide context to authcontext Keep track of the logged-in user and give that information to everything inside it.
export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<User | null>(() => {// remember this current user for me
    const savedUser = localStorage.getItem("onlineExamUser"); //"When my app starts, check whether we previously saved a logged-in user."

    return savedUser ? JSON.parse(savedUser) : null;
  });

  useEffect(() => {
    if (user) { //if someone loged in
      //Browser has something called localStorage. think of it as small storage box
      localStorage.setItem("onlineExamUser", JSON.stringify(user)); //save user info in browser
    } else {
      localStorage.removeItem("onlineExamUser"); //if no one loged remove user
    }
  }, [user]);

  function login(user: User) {
    setUser(user);
  }

  function logout() {
    setUser(null);
  }

  ////means give these 3 things to everyone inside it
  return (
    <AuthContext.Provider value={{ user, login, logout }}> 
      {children}
    </AuthContext.Provider>
  );
}