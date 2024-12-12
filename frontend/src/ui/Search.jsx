import { CiSearch } from "react-icons/ci";
import { useQuery } from "./../context/SearchQuery";
import { useEffect, useRef } from "react";

function Search() {
  const { query, setQuery } = useQuery();
  const inputEl = useRef(null);

  useEffect(
    function () {
      function callback(e) {
        if (document.activeElement === inputEl.current) return;

        if (e.code === "Enter") {
          inputEl.current.focus();
          setQuery("");
        }
      }

      document.addEventListener("keydown", callback);

      return () => document.removeEventListener("keydown", callback);
    },
    [setQuery]
  );

  return (
    <div className="ml-8 flex flex-row items-center my-4 ">
      <CiSearch size={40} className="dark:text-white" />

      <input
        type="text"
        placeholder="Search for contacts,groups"
        className="p-2 w-full outline-none dark:bg-cyan-950"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        ref={inputEl}
      />
    </div>
  );
}

export default Search;
