import { useState } from 'react'

export default function Search() {
   const [searchText, setSearchText] = useState('')

   const handleSubmit = (e: any) => {
       e.preventDefault()
       location.href = `/search?search=${searchText}`
   }

   return (
      <div className="widget">
         <h5 className="widget-title"><span>Search</span></h5>
         <form className="widget-search"
            onSubmit={handleSubmit}
         >
            <input id="search-query2" name="s" type="search" placeholder="Type &amp; Hit Enter..." 
             value={searchText}
             onChange={(e) => setSearchText(e.target.value)}
            />
            <button type="submit">
               <i className="ti-search"></i>
            </button>
         </form>
      </div>
   )
}

