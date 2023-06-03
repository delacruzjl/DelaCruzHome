import { useState } from 'react'

export default function Newsletter() {
    const [email, setEmailText] = useState('')

    const handleSubmit = (e: any) => {
        e.preventDefault()
        location.href = `/search?search=${email}`
    }

    return (
        <div className="column is-3-desktop">
             <h6 className="mb-4">Subscribe Newsletter</h6>
             <form className="subscription"
              onSubmit={handleSubmit}>
                <div className="is-relative">
                   <i className="ti-email email-icon"></i>
                   <input type="email" className="input" placeholder="Your Email Address"
                    value={email}
                    onChange={(e) => setEmailText(e.target.value)}
                   />
                </div>
                
                <article className="message is-danger">
                    <div className="message-header">
                        <p>Unavailable</p>
                        <button className="delete" aria-label="delete"></button>
                    </div>
                    <div className="message-body">
                        Unfortunately we are not accepting new subscribers at this time.
                    </div>
                </article>
                
                <button className="btn btn-primary w-100 rounded mt-2" type="submit">Subscribe now</button>
             </form>
          </div>
    )
}