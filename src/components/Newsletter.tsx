import { useState } from 'react'

export default function Newsletter() {
    const [email, setEmailText] = useState('')

    const handleSubmit = async (e: any) => {
        e.preventDefault()
        const data = { 'email': email }
        
        const response = await fetch('/api/NewsletterSubscriber', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })

        console.log(response)
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
                
                <button className="btn btn-primary w-100 rounded mt-2" type="submit">Subscribe now</button>
             </form>
          </div>
    )
}