import { useState } from 'react'

export default function Newsletter() {
    const [email, setEmailText] = useState('')
    const [hasErrors, setHasErrors] = useState(false)
    const [hasSuccess, setHasSuccess] = useState(false)

    const handleSubmit = async (e: any) => {
        e.preventDefault()
        const data = { 'email': email }
        

        const response = await fetch(`/api/NewsletterSubscriber`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })

        setHasErrors(!(response.status >= 400))
        setHasSuccess(response.status < 400)
    }

    return (
        <div className="column is-3-desktop">
             <h6 className="mb-4">Subscribe Newsletter</h6>
             <form className={`subscription ${hasErrors ? 'is-hidden' : 'is-block'}`}
              onSubmit={handleSubmit}>
                <div className="is-relative">
                   <i className="ti-email email-icon"></i>
                   <input type="email" className="input" placeholder="Your Email Address"
                    value={email}
                    onChange={(e) => setEmailText(e.target.value)}
                   />
                </div>
                
                <button className="btn btn-primary w-100 rounded mt-2" type="submit">Subscribe now</button>
             </form>

             <article className={`message is-danger ${hasErrors ? 'is-block' : 'is-hidden'}` }>
                <div className="message-header">
                    <p></p>
                    <button className="delete" aria-label="delete" onClick={(e) => setHasErrors(false)}></button>
                </div>
                <div className="message-body">
                    Oops! Something went wrong. Please try again later.
                </div>
            </article>

            <article className={`message is-primary is-light ${hasSuccess ? 'is-block' : 'is-hidden'}` }>
                <div className="message-header">
                    <p></p>
                    <button className="delete" aria-label="delete" onClick={(e) => setHasSuccess(false)}></button>
                </div>
                <div className="message-body">
                    Thank you, I will keep you updated.
                </div>
            </article>
          </div>
     
     
    )
}