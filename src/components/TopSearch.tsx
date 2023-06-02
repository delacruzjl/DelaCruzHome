import { useState } from 'react'

export default function TopSearch() {
    const [searchText, setSearchText] = useState('')

    const handleSubmit = (e: any) => {
        e.preventDefault()
        location.href = `/search?search=${searchText}`
    }

    return (
        <>
            <form className="h-100"
                onSubmit={handleSubmit}
            >
                <input className="search-box pl-4" id="search-query" name="s" type="search" placeholder="Type &amp; Hit Enter..." 
                    value={searchText}
                    onChange={(e) => setSearchText(e.target.value)}
                />
                        
                <button id="searchClose" className="search-close" type='submit'>
                    <i className="ti-close text-dark"></i>
                </button>
            </form>
        </>
    )
}

