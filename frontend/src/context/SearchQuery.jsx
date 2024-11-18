import { createContext, useState, useContext } from "react";

const QueryContext = createContext();

function useQuery() {
  return useContext(QueryContext);
}

function SelectedQueryProvider({ children }) {
  const [query, setQuery] = useState("");

  return (
    <QueryContext.Provider value={{ query, setQuery }}>
      {children}
    </QueryContext.Provider>
  );
}

export { SelectedQueryProvider, useQuery };
